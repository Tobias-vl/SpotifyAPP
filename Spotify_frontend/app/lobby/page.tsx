"use client";

import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import "./lobby.css"
import { useRouter } from "next/navigation";

export default function LobbyPage() {
    const [lobbyName, setLobbyName] = useState("")
    const router = useRouter();

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
                        <Button className="w-full">Create Lobby</Button>
                    </div>
                    <div className="lobby-buttons">
                        <Button className="w-full">Join Lobby</Button>
                    </div>
                </CardContent>
            </Card>
        </main>
    );
}