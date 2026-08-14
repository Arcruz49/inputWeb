import { createContext, useCallback, useEffect, useMemo, useState } from 'react'
import type { ReactNode } from 'react'
import * as authApi from '../api/auth'
import type { RegisterPayload } from '../api/auth'
import type { SessionUser } from '../types'

const STORAGE_KEY = 'inputweb_user'

export interface AuthContextValue {
  user: SessionUser | null
  loading: boolean
  signIn: (email: string, password: string) => Promise<void>
  signUp: (payload: RegisterPayload) => Promise<void>
  signOut: () => Promise<void>
}

export const AuthContext = createContext<AuthContextValue | null>(null)

function readCache(): Partial<SessionUser> {
  try {
    const raw = localStorage.getItem(STORAGE_KEY)
    return raw ? (JSON.parse(raw) as Partial<SessionUser>) : {}
  } catch {
    return {}
  }
}

function writeCache(user: SessionUser | null) {
  if (user) localStorage.setItem(STORAGE_KEY, JSON.stringify(user))
  else localStorage.removeItem(STORAGE_KEY)
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<SessionUser | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    let active = true

    authApi
      .me()
      .then((profile) => {
        if (!active) return
        const cached = readCache()
        const session: SessionUser = {
          id: profile.id,
          name: profile.name || cached.name || '',
          email: cached.email ?? '',
        }
        setUser(session)
        writeCache(session)
      })
      .catch(() => {
        if (!active) return
        setUser(null)
        writeCache(null)
      })
      .finally(() => {
        if (active) setLoading(false)
      })

    return () => {
      active = false
    }
  }, [])

  const adopt = useCallback((name: string, email: string) => {
    const cached = readCache()
    const session: SessionUser = { id: cached.id ?? '', name, email }
    setUser(session)
    writeCache(session)
  }, [])

  const signIn = useCallback(
    async (email: string, password: string) => {
      const result = await authApi.login(email, password)
      adopt(result.name, result.email)
    },
    [adopt],
  )

  const signUp = useCallback(
    async (payload: RegisterPayload) => {
      const result = await authApi.register(payload)
      adopt(result.name, result.email)
    },
    [adopt],
  )

  const signOut = useCallback(async () => {
    try {
      await authApi.logout()
    } finally {
      setUser(null)
      writeCache(null)
    }
  }, [])

  const value = useMemo<AuthContextValue>(
    () => ({ user, loading, signIn, signUp, signOut }),
    [user, loading, signIn, signUp, signOut],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
