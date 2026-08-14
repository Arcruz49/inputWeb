function toDate(value: string): Date {
  const hasZone = /Z$|[+-]\d{2}:?\d{2}$/.test(value)
  return new Date(hasZone ? value : `${value}Z`)
}

export function formatDateTime(value: string): string {
  const date = toDate(value)
  if (Number.isNaN(date.getTime())) return '—'
  return date.toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

export function formatRelative(value: string): string {
  const date = toDate(value)
  if (Number.isNaN(date.getTime())) return '—'

  const seconds = Math.round((Date.now() - date.getTime()) / 1000)
  if (seconds < 60) return 'agora há pouco'

  const minutes = Math.round(seconds / 60)
  if (minutes < 60) return `há ${minutes} min`

  const hours = Math.round(minutes / 60)
  if (hours < 24) return `há ${hours} h`

  const days = Math.round(hours / 24)
  if (days < 30) return `há ${days} d`

  return formatDateTime(value)
}
