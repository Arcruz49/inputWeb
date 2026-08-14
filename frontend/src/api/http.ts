const BASE = import.meta.env.VITE_API_BASE ?? '/api'

export class ApiError extends Error {
  status: number

  constructor(status: number, message: string) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

interface ErrorBody {
  message?: string
  error?: string
  title?: string
}

function messageFromBody(raw: string, fallback: string): string {
  if (!raw) return fallback
  try {
    const body = JSON.parse(raw) as ErrorBody
    return body.message || body.error || body.title || fallback
  } catch {
    // 500 sem middleware, HTML de erro do proxy, etc.
    return raw.length > 200 ? fallback : raw
  }
}

function defaultMessage(status: number): string {
  if (status === 401) return 'Sessão expirada. Faça login novamente.'
  if (status === 403) return 'Você não tem permissão para isso.'
  if (status === 404) return 'Não encontrado.'
  if (status === 413) return 'Arquivo grande demais para o servidor.'
  if (status >= 500) return 'Erro no servidor. Tente novamente.'
  return `Falha na requisição (${status}).`
}

export async function api<T>(path: string, init: RequestInit = {}): Promise<T> {
  const isJsonBody = init.body !== undefined && !(init.body instanceof FormData)

  let res: Response
  try {
    res = await fetch(`${BASE}${path}`, {
      credentials: 'include',
      ...init,
      headers: {
        Accept: 'application/json',
        ...(isJsonBody ? { 'Content-Type': 'application/json' } : {}),
        ...init.headers,
      },
    })
  } catch {
    throw new ApiError(0, 'Não foi possível falar com a API.')
  }

  const raw = res.status === 204 ? '' : await res.text()

  if (!res.ok) {
    throw new ApiError(res.status, messageFromBody(raw, defaultMessage(res.status)))
  }

  return (raw ? (JSON.parse(raw) as T) : (undefined as T))
}
