"use client";

import { useState } from "react"
import { useLobbyEvents } from "@/hooks/useSocketMessages";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import "./lobby.css"
import { useRouter } from "next/navigation";
import { useUserId } from "@/hooks/useUserId"
import { URL } from "url";

export default function LobbyPage() {
    useLobbyEvents();

    const backendBaseUrl = process.env.NEXT_PUBLIC_BACKEND_URL ?? "http://localhost:7115";

    
    const [lobbyName, setLobbyName] = useState("")
    const { userId, isLoading } = useUserId()
    const [StatusText, setStatustext] = useState("Create Lobby")
    const [JoinText, setJoinText] = useState("Join Lobby")
    const [isCreating, setIsCreating] = useState(false)
    const [isJoining, setIsJoining] = useState(false)
    const router = useRouter();

    async function CreateLobby(){

        console.log("test");

        if (isLoading || !userId) {
            setStatustext("Loading user info...")
            return
        }
        if (isCreating) return
        
        if (lobbyName == null || lobbyName.trim() === "") {
            setStatustext("Please enter a Lobby Name")
            return
        } 
        if (lobbyName.length >= 33){
            setStatustext("Lobby Name can only be up to 32 characters")
            return
        }
        if (!(/^[a-zA-Z]*$/.test(lobbyName))){
            setStatustext("Lobby Name can only be \"normal characters\"")
            return
        }
        
        setIsCreating(true)
        setStatustext("Creating Lobby...")

        try {
            const response = await fetch(`${backendBaseUrl}/create`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    lobbyName: lobbyName,
                    userId: userId
                })
            })

            if (!response.ok) {
                setStatustext("Failed to create lobby")
                setIsCreating(false)
                return
            }

            const data = await response.json()
            setStatustext("Lobby created! Redirecting...")
            
            // Redirect to the new lobby
            router.push(`/lobby/${data.lobbyId}`)
        } catch (error) {
            console.error("Error creating lobby:", error)
            setStatustext("Error creating lobby. Try again.")
            setIsCreating(false)
        }
    }

    async function JoinLobby(){
        if (isLoading || !userId) {
            setJoinText("Loading user info...")
            return
        }
        
        if (isJoining) return
        
        if (lobbyName == null || lobbyName.trim() === "") {
            setJoinText("Please enter a Lobby ID to join")
            return
        }

        try {
            setIsJoining(true)
            setJoinText("Joining Lobby...")

            const response = await fetch(`${backendBaseUrl}/${lobbyName}/join`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ userId })
            })

            if (!response.ok) {
                setJoinText("Failed to join lobby")
                setIsJoining(false)
                return
            }

            setJoinText("Lobby joined! Redirecting...")
            // Redirect to the lobby (use the entered lobbyName as the ID)
            router.push(`/lobby/${lobbyName}`)
        } catch (error) {
            console.error("Error joining lobby:", error)
            setJoinText("Error joining lobby. Try again.")
            setIsJoining(false)
        }
    }

    


    return (
        <main className="lobby-page">

                    <div className="lobby-buttons">
                        <Button className="w-full"  onClick={() => router.push("/dev")} >dev page </Button>
                    </div>

            <Card className="mx-auto w-full max-w-md shadow-md">
                <CardHeader>
                    <CardTitle>Lobby</CardTitle>
                    <CardDescription>Create or join a lobby to start listening together</CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                    <input
                        className="lobby-input"
                        placeholder="Lobby name"
                        value={lobbyName}
                        onChange={(e) => setLobbyName(e.target.value)}
                    />
                    <div className="lobby-buttons">
                        <Button className="w-full" onClick={CreateLobby} disabled={isLoading || isCreating}>{StatusText}</Button>
                    </div>
                    <div className="lobby-buttons">
                        <Button className="w-full" onClick={JoinLobby} disabled={isLoading || isJoining}>{JoinText}</Button>
                    </div>
                </CardContent>
            </Card>
        </main>
    );
}