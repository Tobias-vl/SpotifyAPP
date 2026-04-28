"use client";

import { useState, useEffect } from "react";
import { useParams, useRouter } from "next/navigation";
import { useUserId } from "@/hooks/useUserId";
import { useLobbyEvents } from "@/hooks/useSocketMessages";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

type Member = string | { name: string; voted: boolean };

export default function LobbyDetailPage() {
  const params = useParams();
  const lobbyId = params.id as string;
  const { userId } = useUserId();
  const router = useRouter();
  
  useLobbyEvents();

  const [lobbyData, setLobbyData] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [members, setMembers] = useState<Member[]>([]);
  const backendBaseUrl = process.env.NEXT_PUBLIC_BACKEND_URL ?? "http://localhost:7115";
  const getMemberName = (member: Member | undefined) => {
    if (!member) return undefined;
    return typeof member === 'string' ? member : member.name;
  };
  const createdByDisplayName =
    lobbyData?.hostName ||
    lobbyData?.hostUserId ||
    lobbyData?.userId ||
    getMemberName(members[0]) ||
    "Unknown";

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
          <h1>Lobby Name: {lobbyData.lobbyName}</h1>
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
                <p className ="Lobby-info-value"> Lobby ID: {lobbyId}</p>
                <p className="lobby-info-label">Created by</p>
                <p className="lobby-info-value">{createdByDisplayName}</p>
              </div>

              <CardHeader>
              <CardTitle>Members ({members.length})</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="member-list">
                {members.length === 0 ? (
                  <p style={{ color: "#999" }}>No members yet</p>
                ) : (
                  members.map((member) => {
                    const memberName = typeof member === 'string' ? member : member.name;
                    const memberKey = typeof member === 'string' ? member : member.name;
                    return (
                      <div 
                        key={memberKey}
                        className="member-item"
                      >
                        <div className="member-status" />
                        {memberName} {memberName === userId && "(You)"}
                      </div>
                    );
                  })
                )}
              </div>
            </CardContent>

              <div className="buttons">
              <Button 
                onClick={leaveLobby}
                className="button-leave"
              >
                Leave Lobby
              </Button>

              <Button
                className="button-play"
                >
                  Start Game
              </Button>

              </div>

            </CardContent>
          </Card>
        </div>
      </div>
    </main>
  );
}