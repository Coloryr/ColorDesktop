import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import { Plugin } from 'rollup';

export function preserveFilesPlugin(filesToPreserve: string[]): Plugin {
  return {
    name: 'preserve-files',

    transform(code, id) {
      const shouldPreserve = filesToPreserve.some(file => id.endsWith(file));
      if (shouldPreserve) {
        return {
          code,
          map: null,
          moduleSideEffects: 'no-treeshake',
        };
      }
      return null;
    }
  };
}

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin()],
  server: {
    port: 50829,
  },
  esbuild: {
    keepNames: true
  },
  optimizeDeps: {

  },
  build: {
    minify: false,
    rollupOptions: {
      plugins: [
        preserveFilesPlugin([
          'src/api/colordesktop.ts',
          'src/api/enums.ts',
          'src/api/instance.ts'
        ])
      ]
    }
  }
})
