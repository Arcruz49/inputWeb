import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

// Em dev o Vite faz o papel do nginx: /api/* vira uma chamada direta na API.
// Assim o browser enxerga tudo como mesma origem e o cookie HttpOnly funciona.
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')
  const target = env.API_URL || 'http://localhost:5143'

  return {
    plugins: [react()],
    server: {
      port: 5173,
      proxy: {
        '/api': {
          target,
          changeOrigin: true,
          secure: false,
          rewrite: (path) => path.replace(/^\/api/, ''),
        },
      },
    },
  }
})
