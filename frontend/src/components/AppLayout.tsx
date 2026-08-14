import { NavLink, Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/useAuth'
import { IconList, IconLogout } from './Icons'

function initials(name: string) {
  const parts = name.trim().split(/\s+/).filter(Boolean)
  if (parts.length === 0) return '?'
  if (parts.length === 1) return parts[0].slice(0, 2).toUpperCase()
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase()
}

export function AppLayout() {
  const { user, signOut } = useAuth()
  const navigate = useNavigate()

  async function handleLogout() {
    await signOut()
    navigate('/login', { replace: true })
  }

  return (
    <div className="shell">
      <aside className="sidebar">
        <div className="sidebar-head">
          <span className="brand">
            Input<span className="dot">.</span>
          </span>
        </div>

        <nav className="sidebar-nav">
          <NavLink to="/records" className="nav-item">
            <IconList />
            <span>Gravações</span>
          </NavLink>
        </nav>

        <div className="sidebar-foot">
          <div className="avatar">{initials(user?.name ?? '')}</div>
          <div className="who">
            <div className="who-name">{user?.name || 'Usuário'}</div>
            {user?.email ? <div className="who-mail">{user.email}</div> : null}
          </div>
          <button type="button" className="icon-btn" onClick={handleLogout} title="Sair">
            <IconLogout />
          </button>
        </div>
      </aside>

      <main className="main">
        <Outlet />
      </main>
    </div>
  )
}
