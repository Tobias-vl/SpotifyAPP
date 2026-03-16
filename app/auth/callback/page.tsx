"use client";

import { useEffect, useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";

export default function AuthCallbackPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [message, setMessage] = useState("Finalizing login...");

  useEffect(() => {
    const userId = searchParams.get("userId");

    if (!userId) {
      setMessage("Missing token in callback. Please try logging in again.");
      return;
    }

    localStorage.setItem("spotify.userId", userId);

    setMessage("Login successful. Redirecting to lobby...");
    router.replace("/lobby");
  }, [router, searchParams]);

  return (
    <main style={{ minHeight: "100vh", display: "grid", placeItems: "center", padding: "1rem" }}>
      <p>{message}</p>
    </main>
  );
}
