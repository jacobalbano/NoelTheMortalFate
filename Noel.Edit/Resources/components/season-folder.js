Vue.component('season-folder', {
    props: ['number', 'files', 'filter'],
    data: function () {
        return {
            number: 0,
            files: [],
            filter: ''
        };
    },

    computed: {
        filteredFiles: function() {
            const upFilter = (this.filter || '').toUpperCase();
            return _.filter(this.files, x => x.toUpperCase().indexOf(upFilter) >= 0);
		}
	},
    template: `
    <div v-if="filteredFiles.length > 0">
	    <p class="menu-list">Season {{number}}</p>
	    <ul class="menu-list">
		    <li v-for="file in filteredFiles" @click="$emit('file-clicked', number, file)">
                <a>{{file}}</a>
            </li>
	    </ul>
	</div>`
});