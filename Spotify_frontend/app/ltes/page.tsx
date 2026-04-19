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
            </Card>
        </main>
    );
}