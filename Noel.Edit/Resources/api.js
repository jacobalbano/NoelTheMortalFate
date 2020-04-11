window.api = (function() {
	"use strict";
	
	return {
		getFiletree,
		getFullTextSearch,

		getFile,
		saveFile,
	};
	
	function getFiletree() {
		return httpGet('/api/Filetree');
	}

	function getFullTextSearch(term) {
		return httpGet('/api/FullTextSearch', { term });
	}

	async function getFile(seasonNum, filename) {
		if (seasonNum == undefined) throw 'arg-null: seasonNum';
		if (filename == undefined) throw 'arg-null: filename';

		//	massage data to fit HOT
		const file = await httpGet('/api/TranslationFile', { seasonNum, filename });
		return {
			file: file,
			isError: false,
			isSaving: false
		};
	}

	async function saveFile(file) {
		if (file == undefined) throw 'arg-null: file';

		const [result, _] = await Promise.all([
			httpPost('/api/TranslationFile', file),
			later(1000)
		]);

		return result;
	}
	
	async function httpGet(route, args) {
		if (args !== undefined)
			args = '?' + Object.keys(args)
				.map(key => encodeURIComponent(key) + '=' + encodeURIComponent(args[key]))
				.join('&');

		var result = await fetch(window.location + route + (args || ''));
		if (result.status == 500) {
			const ex = await result.json();
			throw ex.Message;
		}

		return result.json();
	}
	
	async function httpPost(route, payload) {
		var result = await fetch(window.location + route, {
			method : "POST",
			body : JSON.stringify(payload)
		});

		if (result.status == 500) {
			const ex = await result.json();
			throw ex.Message;
		}

		return result;
	}

	function later(delay) {
		return new Promise(function(resolve) {
			setTimeout(resolve, delay);
    });
}
})();