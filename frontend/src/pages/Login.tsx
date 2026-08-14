import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { AuthAside } from '../components/AuthAside'
import { IconEye, IconEyeOff } from '../components/Icons'
import { useAuth } from '../auth/useAuth'

export function Login() {
  const { signIn } = useAuth()
  const navigate = useNavigate()

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    if (submitting) return

    setError('')
    setSubmitting(true)
    try {
      await signIn(email.trim(), password)
      navigate('/records', { replace: true })
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Não foi possível entrar.')
      setSubmitting(false)
    }
  }

  return (
    <div className="auth">
      <AuthAside />

      <section className="auth-panel">
        <form className="auth-form" onSubmit={handleSubmit} noValidate>
          <span className="eyebrow">Bem-vindo de volta</span>
          <h2>Acesse sua conta</h2>

          <div className="auth-fields">
            <div className="field">
              <label htmlFor="email">E-mail</label>
              <input
                id="email"
                type="email"
                autoComplete="email"
                placeholder="voce@email.com"
                value={email}
                className={error ? 'invalid' : undefined}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>

            <div className="field">
              <label htmlFor="password">Senha</label>
              <div className="pwd-wrap">
                <input
                  id="password"
                  type={showPassword ? 'text' : 'password'}
                  autoComplete="current-password"
                  placeholder="••••••••"
                  value={password}
                  className={error ? 'invalid' : undefined}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
                <button
                  type="button"
                  className="eye-btn"
                  tabIndex={-1}
                  onClick={() => setShowPassword((v) => !v)}
                  aria-label={showPassword ? 'Ocultar senha' : 'Mostrar senha'}
                >
                  {showPassword ? <IconEyeOff /> : <IconEye />}
                </button>
              </div>
            </div>
          </div>

          {error ? <div className="alert alert-error">{error}</div> : null}

          <button type="submit" className="btn btn-primary btn-block" disabled={submitting}>
            {submitting ? <span className="spinner" /> : 'Entrar'}
          </button>

          <p className="auth-switch">
            Ainda não tem conta?{' '}
            <Link to="/register" className="link">
              Criar conta
            </Link>
          </p>
        </form>
      </section>
    </div>
  )
}
