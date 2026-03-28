"use client";

import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { useUserId } from "@/hooks/useUserId";
import { useLobbyEvents } from "@/hooks/useSocketMessages";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function LobbyDetailPage() {
  const params = useParams();
  const lobbyId = params.id as string;
  const { userId } = useUserId();
  const router = useRouter();
  
  useLobbyEvents();

  const [lobbyData, setLobbyData] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [members, setMembers] = useState<string[]>([]);
  const backendBaseUrl = process.env.NEXT_PUBLIC_BACKEND_URL ?? "http://localhost:7115";

  useEffect(() => {
    // Fetch lobby details
    const fetchLobby = async () => {
      try {
        console.log("Fetching lobby:", lobbyId);
        const response = await fetch(`${backendBaseUrl}/${lobbyId}`);
        
        if (!response.ok) {
          console.error("Lobby not found");
          router.push("/lobby");
          return;
        }
        
        const data = await response.json();
        setLobbyData(data);
        setMembers(data.membersUserId || []);
      } catch (error) {
        console.error("Error fetching lobby:", error);
        router.push("/lobby");
      } finally {
        setIsLoading(false);
      }
    };

    if (lobbyId && userId) {
      fetchLobby();
    }
  }, [lobbyId, userId, backendBaseUrl, router]);

  async function leaveLobby() {
    try {
      const response = await fetch(`${backendBaseUrl}/${lobbyId}/leave`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userId })
      });

      if (response.ok) {
        router.push("/lobby");
      }
    } catch (error) {
      console.error("Error leaving lobby:", error);
    }
  }

  if (isLoading) {
    return (
      <main>
        <div className="lobby-container">
          <p>Loading lobby...</p>
        </div>
      </main>
    );
  }

  if (!lobbyData) {
    return (
      <main>
        <div className="lobby-container">
          <p>Lobby not found</p>
        </div>
      </main>
    );
  }

  return (
    <main>
      <div className="lobby-container">
        {/* Header */}
        <div className="lobby-header">
          <h1>{lobbyData.lobbyName}</h1>
          <p>Lobby ID: {lobbyId}</p>
          <Button onClick={() => router.push("/lobby")} variant="outline">
            Back to Lobbies
          </Button>
        </div>

        {/* Main Content */}
        <div className="lobby-content">
          {/* Lobby Info */}
          <Card>
            <CardHeader>
              <CardTitle>Lobby Info</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="lobby-info-item">
                <p className="lobby-info-label">Created by</p>
                <p className="lobby-info-value">{lobbyData.hostUserId || "Unknown"}</p>
              </div>
              <div className="lobby-info-item">
                <p className="lobby-info-label">Created at</p>
                <p className="lobby-info-value">{new Date(lobbyData.createAt).toLocaleDateString()}</p>
              </div>
              <Button 
                onClick={leaveLobby}
                className="button-leave"
              >
                Leave Lobby
              </Button>
            </CardContent>
          </Card>

          {/* Members */}
          <Card>
            <CardHeader>
              <CardTitle>Members ({members.length})</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="member-list">
                {members.length === 0 ? (
                  <p style={{ color: "#999" }}>No members yet</p>
                ) : (
                  members.map((member) => (
                    <div 
                      key={member}
                      className="member-item"
                    >
                      <div className="member-status" />
                      {member} {member === userId && "(You)"}
                    </div>
                  ))
                )}
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Music Player Placeholder */}
        <Card className="lobby-info">
          <CardHeader>
            <CardTitle>Now Playing</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="empty-state">
              Music player coming soon...
            </p>
          </CardContent>
        </Card>
      </div>
    </main>
  );
}