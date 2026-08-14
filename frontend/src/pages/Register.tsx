import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { AuthAside } from '../components/AuthAside'
import { IconEye, IconEyeOff } from '../components/Icons'
import { useAuth } from '../auth/useAuth'

export function Register() {
  const { signUp } = useAuth()
  const navigate = useNavigate()

  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [birthDate, setBirthDate] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    if (submitting) return

    if (password.length < 6) {
      setError('A senha deve ter pelo menos 6 caracteres.')
      return
    }

    setError('')
    setSubmitting(true)
    try {
      await signUp({
        name: name.trim(),
        email: email.trim(),
        password,
        birthDate: new Date(`${birthDate}T00:00:00Z`).toISOString(),
      })
      navigate('/records', { replace: true })
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Não foi possível criar a conta.')
      setSubmitting(false)
    }
  }

  return (
    <div className="auth">
      <AuthAside />

      <section className="auth-panel">
        <form className="auth-form" onSubmit={handleSubmit} noValidate>
          <span className="eyebrow">Primeiro acesso</span>
          <h2>Criar conta</h2>

          <div className="auth-fields">
            <div className="field">
              <label htmlFor="name">Nome</label>
              <input
                id="name"
                type="text"
                autoComplete="name"
                placeholder="Seu nome"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
              />
            </div>

            <div className="field">
              <label htmlFor="email">E-mail</label>
              <input
                id="email"
                type="email"
                autoComplete="email"
                placeholder="voce@email.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>

            <div className="field">
              <label htmlFor="birthDate">Data de nascimento</label>
              <input
                id="birthDate"
                type="date"
                value={birthDate}
                max={new Date().toISOString().slice(0, 10)}
                onChange={(e) => setBirthDate(e.target.value)}
                required
              />
            </div>

            <div className="field">
              <label htmlFor="password">Senha</label>
              <div className="pwd-wrap">
                <input
                  id="password"
                  type={showPassword ? 'text' : 'password'}
                  autoComplete="new-password"
                  placeholder="mínimo 6 caracteres"
                  value={password}
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
            {submitting ? <span className="spinner" /> : 'Criar conta'}
          </button>

          <p className="auth-switch">
            Já tem conta?{' '}
            <Link to="/login" className="link">
              Entrar
            </Link>
          </p>
        </form>
      </section>
    </div>
  )
}
