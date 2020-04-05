app = new Vue({
  el: '#vue-app',
  data: {
    files: [],
    activeFile: null,

    tableSettings: {
	    rowHeaders: true,
	    colHeaders: ['Address', 'Meta', 'Source value', 'Patch value'],
	    columns: [
		    { data: 'address' },
		    { data: 'instructions'},
		    { data: 'sourceValue', readOnly: true },
		    { data: 'patchValue' }
	    ],
	    stretchH: 'last',
	    comments: true,
		  
	    filters: true,
	    dropdownMenu: false,
	    hiddenColumns: {
		    columns: [0, 1],
		    indicators: false
	    },

	    licenseKey: 'non-commercial-and-evaluation'
    }
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

  components: {
  	  HotTable : Handsontable.vue.HotTable
  }
})

/*
return;
	
hot.addHook('afterChange', function(changes, source) {
//after every change, run validation on the "0 column"
var instance = this;
var column = instance.getDataAtCol(2);
column.forEach(function(value, row) {
var data = _.extend([], column);

    var index = data.indexOf(value);
    data.splice(index, 1);
    var second_index = data.indexOf(value);
    var cell= instance.getCellMeta(row, 2);
    if (index > -1 && second_index > -1 && !(value == null || value === '')) {
        comments ='true'
        cell.comment ={value: 'Error: No Duplicate Value allowed !!!'}
    } else {
        cell.comment = ''
    }
});
//force a re-render so the new cell properties show up
hot.render()
});
*/