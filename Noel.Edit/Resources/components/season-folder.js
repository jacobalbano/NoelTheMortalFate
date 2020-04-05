Vue.component('season-folder', {
    props: ['number', 'files', 'filter'],
    data: function () {
        return {
            number: 0,
            files: [],
            filter: '',
            expanded: false
        };
    },

    computed: {
        filteredFiles: function() {
            const upFilter = (this.filter || '').toUpperCase();
            return _.filter(this.files, x => x.toUpperCase().indexOf(upFilter) >= 0);
		}
	},
    template: `
    <li v-if="filteredFiles.length > 0">
	    <a @click="expanded = !expanded">Season {{number}}</a>
	    <ul v-if="expanded" class="menu-list">
		    <li v-for="file in filteredFiles" @click="$emit('file-clicked', number, file)">
                <a>{{file}}</a>
            </li>
	    </ul>
	</li>`
});