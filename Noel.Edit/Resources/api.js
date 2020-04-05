window.api = (function() {
	"use strict";
	
	return {
		getFiletree,

		getFile,
		saveFile,
	};
	
	function getFiletree() {
		return httpGet('/api/Filetree');
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

	function saveFile(file) {
		if (file == undefined) throw 'arg-null: file';
		return httpPost('/api/TranslationFile', file);
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