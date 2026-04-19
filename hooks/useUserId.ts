"use client";

import { useEffect, useState } from "react";

export function useUserId() {
  const [userId, setUserId] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedUserId = localStorage.getItem("spotify.userId");
    setUserId(storedUserId);
    setIsLoading(false);
  }, []);

  return { userId, isLoading };
}
