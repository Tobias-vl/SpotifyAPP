"use client";

import { FormEvent, useState } from "react";
import { useSocketMessages } from "@/hooks/useSocketMessages";

export function SocketDemo() {
  const [input, setInput] = useState("");
  const { connected, messages, sendMessage } = useSocketMessages();

  const onSend = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!input.trim()) {
      return;
    }

    sendMessage(input);
    setInput("");
  };

  return (
    <main className="mx-auto flex min-h-screen w-full max-w-2xl flex-col gap-6 px-6 py-12">
      <h1 className="text-2xl font-semibold">SignalR Frontend Test</h1>
      <p className="text-sm text-zinc-600">
        Status: {connected ? "Connected" : "Disconnected"}
      </p>

      <form onSubmit={onSend} className="flex gap-2">
        <input
          value={input}
          onChange={(event) => setInput(event.target.value)}
          placeholder="Type and send a message"
          className="w-full rounded border border-zinc-300 px-3 py-2 text-sm outline-none focus:border-zinc-700"
        />
        <button
          type="submit"
          className="rounded bg-black px-4 py-2 text-sm text-white disabled:opacity-50"
          disabled={!connected}
        >
          Send
        </button>
      </form>

      <section className="rounded border border-zinc-200 p-4">
        <h2 className="mb-3 text-sm font-medium">Events</h2>
        <ul className="space-y-2 text-sm text-zinc-700">
          {messages.length === 0 ? (
            <li>No messages yet.</li>
          ) : (
            messages.map((message, index) => <li key={`${message}-${index}`}>{message}</li>)
          )}
        </ul>
      </section>
    </main>
  );
}
