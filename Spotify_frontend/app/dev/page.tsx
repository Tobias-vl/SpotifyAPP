"use client";

import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import "./dev.css"

export default function LobbyPage() {
    const [userId, setUserId] = useState("")
    const [responseText, setResponseText] = useState("")
    const [isLoading, setIsLoading] = useState(false)

    const apiBaseUrl = process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5058"

    async function handleGetPlaylists() {
        if (!userId.trim()) {
            setResponseText("Please enter a userId first.")
            return
        }

        setIsLoading(true)
        setResponseText("Loading...")

        try {
            const endpoint = `${apiBaseUrl}/playlist/${encodeURIComponent(userId.trim())}`
            const response = await fetch(endpoint, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            })

            const text = await response.text()

            if (!response.ok) {
                setResponseText(`Request failed (${response.status}): ${text}`)
                return
            }

            try {
                const data = JSON.parse(text)
                setResponseText(JSON.stringify(data, null, 2))
            } catch {
                setResponseText(text)
            }
        } catch (error) {
            setResponseText(`Network error: ${error instanceof Error ? error.message : "Unknown error"}`)
        } finally {
            setIsLoading(false)
        }
    }

    async function handleTracks() {
        if (!userId.trim()) {
            setResponseText("Please enter a userId first.")
            return
        }

        setIsLoading(true)
        setResponseText("Loading...")

        try {
            const endpoint = `${apiBaseUrl}/GetTracks/${encodeURIComponent(userId.trim())}`
            const response = await fetch(endpoint, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            })

            const text = await response.text()

            if (!response.ok) {
                setResponseText(`Request failed (${response.status}): ${text}`)
                return
            }

            try {
                const data = JSON.parse(text)
                setResponseText(JSON.stringify(data, null, 2))
            } catch {
                setResponseText(text)
            }
        } catch (error) {
            setResponseText(`Network error: ${error instanceof Error ? error.message : "Unknown error"}`)
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <main className="lobby-page">
            <Card className="mx-auto w-full max-w-md shadow-md">
                <CardHeader>
                    <CardTitle>Dev API Tester</CardTitle>
                    <CardDescription>Call backend endpoints and inspect response payloads</CardDescription>
                </CardHeader>

                <CardContent className="space-y-4">
                    <input
                        className="lobby-input"
                        placeholder="User ID"
                        value={userId}
                        onChange={(e) => setUserId(e.target.value)}
                    />

                    <div className="lobby-buttons">
                        <Button className="w-full" onClick={handleGetPlaylists} disabled={isLoading}>
                            {isLoading ? "Loading..." : "GetPlaylists"}
                        </Button>
                    </div>


                    <div className="lobby-buttons">
                        <Button className="w-full" onClick={handleTracks} disabled={isLoading}>
                            {isLoading ? "Loading..." : "GetTracks"}
                        </Button>
                    </div>

                    <textarea
                        className="lobby-input"
                        rows={10}
                        readOnly
                        value={responseText}
                        placeholder="Backend response will appear here"
                    />
                </CardContent>
            </Card>

        </main>
    );
};