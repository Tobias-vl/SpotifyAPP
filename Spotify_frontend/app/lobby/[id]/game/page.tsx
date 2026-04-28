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
  const [selectedVote, setSelectedVote] = useState<string | null>(null);
  const [status, setStatus] = useState<any>(null);
  const [members, setMembers] = useState<Member[]>([]);
  const [Currentsong, setCurrentSong] = useState("No music playing")
  const [CurrentArtist, setCurrentArtist] = useState("No music playing")
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

  function handleVote(member: string) {
    setSelectedVote(member);
    setStatus(`You voted for ${member}.`);
  }

  async function StartNextRound(){

    try{
        if (!lobbyData?.hostUserId) {
            setCurrentSong("Could not fetch song")
            setCurrentArtist("Could not fetch artist")
            setStatus("Host user was not found.")
            return
        }

        const response = await fetch(`${backendBaseUrl}/Skip/${lobbyData.hostUserId}`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
        })

        if (!response.ok){
            setStatus(`Skip failed (${response.status}).`)
            return
        } else {
            setStatus("Skipped to next round.")
            await new Promise(resolve => setTimeout(resolve, 200));
            CurrentSong();
        }
    }
    catch {
      setStatus("Network error while trying to skip track.")
    }

  }

  async function CurrentSong(){
    try{
        if (!lobbyData?.hostUserId) {
            setCurrentSong("Could not fetch song")
            setCurrentArtist("Could not fetch artist")
            return
        }

        const response = await fetch(`${backendBaseUrl}/CurrentTrack/${lobbyData.hostUserId}`)

        if (!response.ok){
            setCurrentSong("Could not fetch song")
            setCurrentArtist("Could not fetch artist")
            return 
        }
        const data = await response.json();

        const songName = data?.item?.name ?? "Unknown song"
        const artistNames = Array.isArray(data?.item?.artists)
          ? data.item.artists
              .map((artist: { name?: string }) => artist?.name)
              .filter(Boolean)
              .join(", ")
          : "Unknown artist"

        setCurrentSong(songName)
        setCurrentArtist(artistNames || "Unknown artist")
    }
    catch{
      setCurrentSong("Could not fetch song")
      setCurrentArtist("Could not fetch artist")
    }
  }


  return (
    <main>
      <div className="lobby-container">
        <div className="lobby-header">
          <h1>Game Round</h1>
          <p>{status}</p>
        </div>

        <div className="lobby-content">
          <Card>
            <CardHeader>
              <CardTitle>Current Song</CardTitle>
            </CardHeader>
            <CardContent className="space-y-3">
              <p className="lobby-info-value">{Currentsong}</p>
              <p >{CurrentArtist}</p>

              <div>
                
              <Button
        onClick={() => StartNextRound()}
        type = "button"
        className="button-playback"
        >Next Round
            </Button>
            
            <Button
            onClick={() => StartNextRound}
            type = "button"
            className="button-playback">
              Pause</Button>
              
              
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle>Vote for the owner</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              {members.length === 0 ? (
                <p className="empty-state">No members yet</p>
              ) : (
                <div className="member-list">
                  {members.map((member) => {
                    const memberName = typeof member === 'string' ? member : member.name;
                    const memberKey = typeof member === 'string' ? member : member.name;
                    return (
                      <Button
                        key={memberKey}
                        type="button"
                        variant={selectedVote === memberName ? "secondary" : "outline"}
                        className="vote-button"
                        onClick={() => handleVote(memberName)}
                      >
                        {memberName} {memberName === userId ? "(You)" : ""}
                      </Button>
                    );
                  })}
                </div>
              )}
            </CardContent>
          </Card>
        </div>
      </div>
    </main>
  );
}