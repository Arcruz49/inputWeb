import { useEffect, useMemo, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { listRecords } from '../api/records'
import type { RecordResponse } from '../types'
import { IconChevron, IconInbox, IconSearch } from '../components/Icons'
import { formatDateTime, formatRelative } from '../utils/format'

export function Records() {
  const navigate = useNavigate()
  const [records, setRecords] = useState<RecordResponse[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [query, setQuery] = useState('')

  useEffect(() => {
    let active = true

    listRecords()
      .then((data) => {
        if (active) setRecords(data)
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
  }, [])

  const filtered = useMemo(() => {
    const term = query.trim().toLowerCase()
    if (!term) return records
    return records.filter(
      (r) =>
        r.project_name.toLowerCase().includes(term) || r.user_name.toLowerCase().includes(term),
    )
  }, [records, query])

  const projectCount = useMemo(() => new Set(records.map((r) => r.project_name)).size, [records])
  const latest = records.length > 0 ? records[0].created_at : null

  return (
    <>
      <header className="page-head">
        <div>
          <span className="eyebrow">Biblioteca</span>
          <h1 className="page-title">Gravações</h1>
        </div>
      </header>

      {error ? <div className="alert alert-error">{error}</div> : null}

      {!error && !loading && records.length > 0 ? (
        <section className="stats">
          <div className="card stat">
            <div className="stat-value">{records.length}</div>
            <div className="stat-label">gravações</div>
          </div>
          <div className="card stat">
            <div className="stat-value">{projectCount}</div>
            <div className="stat-label">{projectCount === 1 ? 'projeto' : 'projetos'}</div>
          </div>
          <div className="card stat">
            <div className="stat-value" style={{ fontSize: '1.05rem', paddingTop: 8 }}>
              {latest ? formatRelative(latest) : '—'}
            </div>
            <div className="stat-label">último envio</div>
          </div>
        </section>
      ) : null}

      {records.length > 0 ? (
        <div className="toolbar">
          <div className="search">
            <IconSearch />
            <input
              type="text"
              placeholder="Buscar por projeto ou usuário…"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
          </div>
        </div>
      ) : null}

      <div className="card">
        {loading ? (
          <div style={{ padding: 22, display: 'grid', gap: 14 }}>
            {[0, 1, 2, 3].map((i) => (
              <div key={i} className="skeleton" style={{ width: `${90 - i * 12}%` }} />
            ))}
          </div>
        ) : filtered.length === 0 ? (
          <div className="empty">
            <div className="empty-icon">
              <IconInbox />
            </div>
            {records.length === 0 ? (
              <>
                <h3>Nenhuma gravação ainda</h3>
                <p>Elas aparecem aqui assim que o aplicativo desktop enviar a primeira.</p>
              </>
            ) : (
              <>
                <h3>Nada encontrado</h3>
                <p>Nenhuma gravação corresponde a "{query}".</p>
              </>
            )}
          </div>
        ) : (
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>Projeto</th>
                  <th>Usuário</th>
                  <th>Enviada em</th>
                  <th style={{ width: 48 }} />
                </tr>
              </thead>
              <tbody>
                {filtered.map((record) => (
                  <tr key={record.id} onClick={() => navigate(`/records/${record.id}`)}>
                    <td className="strong">{record.project_name || '(sem nome)'}</td>
                    <td>
                      <span className="chip">{record.user_name || '—'}</span>
                    </td>
                    <td>{formatDateTime(record.created_at)}</td>
                    <td>
                      <span className="row-go">
                        <IconChevron />
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </>
  )
}
