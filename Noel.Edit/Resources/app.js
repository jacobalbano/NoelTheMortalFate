app = new Vue({
  el: '#vue-app',
  data: {
    files: [],
    activeFile: null,

    tableSettings: {
	    rowHeaders: true,
	    colHeaders: ['Address', 'Meta', 'Japanese', 'English'],
	    columns: [
		    { data: 'address' },
		    { data: 'meta'},
		    { data: 'japanese', readOnly: true },
		    { data: 'english' }
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

    openFile: function(seasonNum, filename) {
			const existing = _.find(this.files, x => x.seasonNum === seasonNum && x.filename === filename);
			if (existing !== undefined) {
				this.activeFile = existing;
				return;
			} else {
				const newFile = api.getFile(seasonNum, filename);
				this.files.push(newFile);
				this.activeFile = newFile;
			}
		},

    saveFile: function() {
        const file = this.activeFile;
        file.isSaving = true;

        api.saveFile(file.seasonNum, file.filename, file.tableData)
            .then(() => file.isSaving = false);
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