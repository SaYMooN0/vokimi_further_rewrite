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
			checkFiles('src/routes', false, false);
		},
		handleHotUpdate({ file }: { file: string }) {
			if (file.includes(path.normalize('src/routes'))) {
				console.log(`Hot update: checking ${file}`);
				checkFiles('src/routes', false, false);
			}
		}
	};
};

function checkFiles(dir: string, insideCFolder: boolean, insideTSFolder: boolean) {
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

			if (insideTSFolder && !entry.name.endsWith('.ts')) {
				throw new Error(`Files inside 'ts_' folders must end with '.ts': ${fullPath}`);
			}
		} else if (entry.isDirectory()) {
			const isCFolder = entry.name.startsWith('c_');
			const isTSFolder = entry.name.startsWith('ts_');

			if (insideCFolder && !isCFolder && !isTSFolder) {
				throw new Error(`Subfolders inside 'c_' folders must start with 'c_' or 'ts_': ${fullPath}`);
			}

			if (insideTSFolder && isCFolder) {
				throw new Error(`'ts_' folders cannot contain 'c_' folders: ${fullPath}`);
			}

			checkFiles(
				fullPath,
				insideCFolder || isCFolder,
				insideTSFolder || isTSFolder
			);
		}
	}
}

function createProxyEntry(
	basePath: string,
	port: number,
	customRewrite?: (path: string) => string
) {
	const basePathRegex = `^${basePath}`;

	return {
		[basePathRegex]: {
			target: `http://localhost:${port}/`,
			secure: false,
			changeOrigin: true,
			rewrite: customRewrite ?? ((path) => path.replace(new RegExp(basePathRegex), '')),
		},
	};
}


export default defineConfig({
	plugins: [sveltekit(), devtoolsJson(), forbiddenFileStructurePlugin()],
	server: {
		proxy: {
			//common
			...createProxyEntry('/api/auth', 5177),
			...createProxyEntry('/api/voki-storage', 5178),
			//voki creation
			...createProxyEntry('/api/voki-creation/core', 5180),
			...createProxyEntry('/api/voki-creation/general', 5181),
			//voki taking
			// ...createProxyEntry(, 5191),
			//other
			...createProxyEntry('/api/tags', 5201),
		}
	}
});
