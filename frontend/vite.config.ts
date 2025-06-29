import devtoolsJson from 'vite-plugin-devtools-json';
import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';
import fs from 'fs';
import path from 'path';

const forbiddenFileStructurePlugin = () => {
	return {
		name: 'validate-file-structure',
		buildStart() {
			console.log('Checking file structure...');
			checkFiles('src/routes', false);
		},
		handleHotUpdate({ file }: { file: string }) {
			if (file.includes(path.normalize('src/routes'))) {
				console.log(`Hot update: checking ${file}`);
				checkFiles('src/routes', false);
			}
		}
	};
};

function checkFiles(dir: string, insideCFolder: boolean) {
	const entries = fs.readdirSync(dir, { withFileTypes: true });

	for (const entry of entries) {
		const fullPath = path.join(dir, entry.name);

		if (/[а-яА-ЯёЁ]/.test(entry.name)) {
			throw new Error(`Cyrillic characters are not allowed: ${fullPath}`);
		}

		if (entry.isFile()) {
			if (insideCFolder && entry.name.startsWith('+')) {
				throw new Error(`Files inside 'c_' folders cannot start with '+': ${fullPath}`);
			}
		} else if (entry.isDirectory()) {
			const isCFolder = entry.name.startsWith('c_');

			if (insideCFolder && !isCFolder) {
				throw new Error(`Subfolders inside 'c_' folders must also start with 'c_': ${fullPath}`);
			}

			checkFiles(fullPath, insideCFolder || isCFolder);
		}
	}
}
function createProxyEntry(
	basePath: string,
	port: number,
	customRewrite?: (path: string) => string
) {
	const regex = `^${basePath}`;
	const rewritePrefix = basePath.replace(/^\/api\/[^/]+/, '');

	return {
		[regex]: {
			target: `http://localhost:${port}/`,
			secure: false,
			changeOrigin: true,
			rewrite: customRewrite ?? ((path) => path.replace(new RegExp(regex), rewritePrefix)),
		},
	};
}

export default defineConfig({
	plugins: [sveltekit(), devtoolsJson(), forbiddenFileStructurePlugin()],
	server: {
		proxy: {
			...createProxyEntry('/api/auth', 5177),
			...createProxyEntry('/api/voki-creation/core', 5180),
			...createProxyEntry('/api/voki-creation/general', 5181),
		}
	}
});
