window.api = (function() {
	"use strict";
	
	return {
		listSeasons,

		getFile,
		saveFile,
	};

	function listSeasons() {
		return [
			{ number: 1, files: [ 'Map004', 'Map005', 'Map006' ]},
			{ number: 2, files: [ 'Map001', 'Map002', 'Map003', 'Map004']},
			{ number: 3, files: [ 'Map003', 'CommonEvents' ]},
		];
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