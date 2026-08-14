import { api } from './http'
import type { RecordResponse } from '../types'

export function listRecords() {
  return api<RecordResponse[]>('/Record')
}

export function getRecord(id: string) {
  return api<RecordResponse>(`/Record/${id}`)
}
