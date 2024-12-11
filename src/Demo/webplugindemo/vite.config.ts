import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  esbuild: {
    keepNames: true
  },
  build: {
    minify: true,
    rollupOptions: {
      treeshake: false
    }
  }
})
