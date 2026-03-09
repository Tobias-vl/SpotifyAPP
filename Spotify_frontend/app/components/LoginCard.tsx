"use client";

import * as React from "react"
import { useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Avatar, AvatarFallback } from "@/components/ui/avatar"

export default function SocialAccountLinkingCard() {
const backendBaseUrl = process.env.NEXT_PUBLIC_BACKEND_URL ?? "http://localhost:7115";
const spotifyLoginPath = process.env.NEXT_PUBLIC_SPOTIFY_LOGIN_PATH ?? "/login";
  const [isRedirecting, setIsRedirecting] = useState(false)

  type Account = {
    name: string
    connected: boolean
    icon: React.ReactNode
  }

  const [accounts, setAccounts] = useState<Account[]>([
    { name: "Spotify", connected: true, icon: <Avatar className="h-5 w-5 bg-red-500"><AvatarFallback>S</AvatarFallback></Avatar> },
  ])



  const startSpotifyLogin = () => {
    setIsRedirecting(true)
    const authUrl = new URL(spotifyLoginPath, backendBaseUrl)

    // Redirecting the full browser starts OAuth with Spotify via the backend.
    window.location.assign(authUrl.toString())
  }

  return (
    <Card className="mx-auto w-full max-w-md shadow-md">
      <CardHeader>
        <CardTitle>Linked Accounts</CardTitle>
        <CardDescription>Manage your connected social accounts</CardDescription>
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