$locator.service('$workspace', function() {
    const vm = {
        activeFile: null,
        files: [],

        isActive,
        focusTab,
        deleteTab,
        openFile,
        saveFile

	};

    return {
	    inject: ['$api', '$confirm'],
        instance: vm
	};

	function isActive(tab) {
        return tab === vm.activeFile;
    }

    function focusTab(index) {
        vm.activeFile = vm.files[index];
    }

    async function deleteTab(index) {
        if (vm.activeFile.isDirty) {
            const shouldClose = await vm.$confirm.modal('Discard file changes?');
            if (shouldClose != 'ok')
                return;
		}

        vm.files.splice(index, 1);
        if (vm.files.length == 0)
            vm.activeFile = null;
        else if (index == 0)
            vm.activeFile = vm.files[0];
        else
            vm.activeFile = vm.files[index-1];
    }

	async function openFile(seasonNum, filename) {
		const existing = _.find(vm.files, { file: { seasonNum, filename }});
		if (existing !== undefined) {
			vm.activeFile = existing;
		} else {
			const file = await vm.$api.getFile(seasonNum, filename);
			vm.files.push(vm.activeFile = {
				file,
				isError: false,
				isSaving: false,
				isDirty: false
			});
		}
	}

	async function saveFile() {
		const file = vm.activeFile;
		file.isSaving = true;

		try {
			await vm.$api.saveFile(file.file);
			file.isError = false;
		} catch (e) {
			file.isError = true;
		} finally {
			file.isSaving = false;
			file.isDirty = false;
		}
	}
});