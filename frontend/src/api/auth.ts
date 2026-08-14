import { api } from './http'
import type { MeDto, UserDto } from '../types'

export interface RegisterPayload {
  name: string
  email: string
  password: string
  birthDate: string
}

export function login(email: string, password: string) {
  return api<UserDto>('/Auth/login', {
    method: 'POST',
    body: JSON.stringify({ email, password }),
  })
}

export function register(payload: RegisterPayload) {
  return api<UserDto>('/Auth/register', {
    method: 'POST',
    body: JSON.stringify(payload),
  })
}

export function me() {
  return api<MeDto>('/Auth/me')
}

export function logout() {
  return api<void>('/Auth/logout', { method: 'POST' })
}
