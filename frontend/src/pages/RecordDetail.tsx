import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { getRecord } from '../api/records'
import type { RecordResponse } from '../types'
import { IconArrowLeft } from '../components/Icons'
import { formatDateTime, formatRelative } from '../utils/format'

export function RecordDetail() {
  const { id = '' } = useParams()
  const [record, setRecord] = useState<RecordResponse | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [copied, setCopied] = useState(false)

  useEffect(() => {
    let active = true
    setLoading(true)
    setError('')

    getRecord(id)
      .then((data) => {
        if (active) setRecord(data)
      })
      .catch((err: unknown) => {
        if (active) setError(err instanceof Error ? err.message : 'Falha ao carregar.')
      })
      .finally(() => {
        if (active) setLoading(false)
      })

    return () => {
      active = false
    }
  }, [id])

  async function copyId() {
    try {
      await navigator.clipboard.writeText(id)
      setCopied(true)
      setTimeout(() => setCopied(false), 1600)
    } catch {
      setCopied(false)
    }
  }

  return (
    <>
      <Link to="/records" className="back-link">
        <IconArrowLeft />
        Gravações
      </Link>

      <header className="page-head">
        <div>
          <span className="eyebrow">Gravação</span>
          <h1 className="page-title">{record?.project_name || (loading ? '…' : 'Detalhe')}</h1>
          {record ? (
            <p className="page-sub">Enviada {formatRelative(record.created_at)}.</p>
          ) : null}
        </div>
      </header>

      {error ? <div className="alert alert-error">{error}</div> : null}

      {loading ? (
        <div className="card detail-grid" style={{ padding: 22, display: 'grid', gap: 14 }}>
          {[0, 1, 2].map((i) => (
            <div key={i} className="skeleton" style={{ width: `${80 - i * 15}%` }} />
          ))}
        </div>
      ) : record ? (
        <>
          <div className="card detail-grid">
            <div className="detail-row">
              <span className="detail-key">Projeto</span>
              <span className="detail-val">{record.project_name || '(sem nome)'}</span>
            </div>
            <div className="detail-row">
              <span className="detail-key">Enviada por</span>
              <span className="detail-val">{record.user_name || '—'}</span>
            </div>
            <div className="detail-row">
              <span className="detail-key">Data do envio</span>
              <span className="detail-val">{formatDateTime(record.created_at)}</span>
            </div>
            <div className="detail-row">
              <span className="detail-key">Identificador</span>
              <span className="detail-val mono">
                {record.id}
                <button type="button" className="copy-btn" onClick={copyId}>
                  {copied ? 'copiado' : 'copiar'}
                </button>
              </span>
            </div>
          </div>

        </>
      ) : null}
    </>
  )
}
