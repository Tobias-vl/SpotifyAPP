"use client";

import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

export default function LoginCard() {
  const backendBaseUrl = process.env.NEXT_PUBLIC_BACKEND_URL ?? "http://localhost:7115";
  const spotifyLoginPath = process.env.NEXT_PUBLIC_SPOTIFY_LOGIN_PATH ?? "/login";
  const [isRedirecting, setIsRedirecting] = useState(false)

  const startSpotifyLogin = () => {
    setIsRedirecting(true)
    const authUrl = new URL(spotifyLoginPath, backendBaseUrl)
    window.location.assign(authUrl.toString())
  }

  return (
    <Card className="mx-auto w-full max-w-md shadow-md">
      <CardHeader>
        <CardTitle>Linked Accounts</CardTitle>
        <CardDescription>Choose Login Method</CardDescription>
      </CardHeader>
      <CardContent className="space-y-4">
        <Button
          className="w-full"
          onClick={startSpotifyLogin}
          disabled={isRedirecting}
        >
          {isRedirecting ? "Redirecting to Spotify..." : "Login with Spotify"}
        </Button>
      </CardContent>
    </Card>
  )
}
