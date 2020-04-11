app = new Vue({
    el: '#vue-app',
    data: {
        files: [],
        activeFile: null,
    },

    methods: {
        isActive: function(tab) { return tab === this.activeFile; },
        focusTab: function(index) { this.activeFile = this.files[index]; },
        deleteTab: function(index) {
            this.files.splice(index, 1);
            if (this.files.length == 0)
                this.activeFile = null;
            else if (index == 0)
                this.activeFile = this.files[0];
            else
                this.activeFile = this.files[index-1];
        },

        openFile: async function(seasonNum, filename) {
            const existing = _.find(this.files, x => x.file.seasonNum === seasonNum && x.file.filename === filename);
            if (existing !== undefined) {
                this.activeFile = existing;
            } else {
                const newFile = await api.getFile(seasonNum, filename);
                this.files.push(newFile);
                this.activeFile = newFile;
            }
        },

        saveFile: async function() {
            const file = this.activeFile;
            file.isSaving = true;

            try {
                await api.saveFile(file.file);
                file.isError = false;
            } catch (e) {
                file.isError = true;
            } finally {
                file.isSaving = false
            }
        },
    },
})