$locator.run(['$workspace'], function($workspace) {
	Vue.component('season-folder', {
        props: ['number', 'files', 'filter'],
        data: function () {
            return {
                expanded: false
            };
        },

        methods: {
            fileClicked: function(seasonNum, filename) {
                $workspace.openFile(seasonNum, filename);
		    }
	    },

        computed: {
            filteredFiles: function() {
                const upFilter = (this.filter || '').toUpperCase();
                return _.filter(this.files, x => x.toUpperCase().indexOf(upFilter) >= 0);
		    }
	    },
        template: `
        <template>
            <li v-if="filteredFiles.length > 0">
	            <a @click="expanded = !expanded">Season {{number}}</a>
	            <ul v-if="expanded" class="menu-list">
		            <li v-for="file in filteredFiles" @click="fileClicked(number, file)">
                        <a>{{file}}</a>
                    </li>
	            </ul>
	        </li>
        </template>`
    });
});