window.api = (function() {
	"use strict";
	
	return {
		listSeasons,

		getFile,
		saveFile,
	};
	
	async function httpGet(route, args) {
		if (args !== undefined)
			args = '?' + Object.keys(args).map(function (key) {
				return encodeURIComponent(key) + '=' + encodeURIComponent(args[key]);
			}).join('&');

		var result = await fetch(window.location + route + (args || ''));
		return result.json();
	}
	
	async function httpPost(route, payload) {
		var result = await fetch(window.location + route, {
			method : "POST",
			body : JSON.stringify(payload)
		});

		return result;
	}

	function listSeasons() {
		return httpGet('/api/Seasons');
	}

	function getFile(seasonNum, filename) {
		if (seasonNum == undefined) throw 'arg-null: seasonNum';
		if (filename == undefined) throw 'arg-null: filename';

		//	massage data to fit HOT
		const rawData = mockGetFileData();
		return {
			seasonNum,
			filename,
			tableData: rawData,
			isError: false,
			isSaving: false
		};

		function mockGetFileData() {
			return [
				{ address: '1/2/3', meta: '', japanese: '少し時間をつぶしたほうがいいかもしれない', english: '' },
				{ address: '1/2/4', meta: '', japanese: 'ジリアン君はたしか市街地の出身だったかな？', english: '' },
				{ address: '2/1/3', meta: '', japanese: 'それは演奏そのものとは関係のないことだ。', english: '' },
				{ address: '5/4/4', meta: '', japanese: 'もっと自分のピアノに自信を持つといい。', english: '' },
				{ address: '1/2/3', meta: '', japanese: '少し時間をつぶしたほうがいいかもしれない', english: '' },
			];
		}
	}

	function saveFile(file) {
		if (file == undefined) throw 'arg-null: file';

		return later(5000);
	}

	function later(delay) {
		return new Promise(function(resolve) {
			setTimeout(resolve, delay);
    });
}
})();