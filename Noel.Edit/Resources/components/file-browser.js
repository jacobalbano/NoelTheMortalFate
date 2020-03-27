Vue.component('file-browser', {
    props: ['files'],
    data: function () {
        return {
            number: 0,
            folders: [],
			fileFilter: '',
        };
    },
	
	created: async function() {
		this.folders = await api.listSeasons();
	},

    template: `
    <div class="column is-narrow" style="width: 300px;">
		<div class="box">
			<p class="title is-5">File Browser</p>
			<p class="control has-icons-left">
				<input class="input" type="text" v-model="fileFilter" placeholder="Search">
				<span class="icon is-left">
					<i class="fas fa-search" aria-hidden="true"></i>
				</span>
			</p>
			<hr/>
			<aside class="menu">
				<season-folder v-for="folder in folders"
					:number="folder.number"
					@file-clicked="$emit('file-clicked', ...arguments)"
					:files="folder.files"
					:filter="fileFilter"
							
				></season-folder>
			</aside>
		</div>
	</div>`
});