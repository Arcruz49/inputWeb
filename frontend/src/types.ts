/** Resposta de /Auth/login e /Auth/register (UserDto). */
export interface UserDto {
  name: string
  email: string
  token: string
}

/** Resposta de /Auth/me. */
export interface MeDto {
  id: string
  name: string
}

/** Resposta de /Record e /Record/{id} (RecordResponse — chaves em snake_case). */
export interface RecordResponse {
  id: string
  project_name: string
  user_name: string
  created_at: string
}

export interface SessionUser {
  id: string
  name: string
  email: string
}
