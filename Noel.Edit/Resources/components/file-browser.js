Vue.component('file-browser', {
    props: ['files'],
    data: function () {
        return {
            number: 0,
            filetree: [],
			fileFilter: '',
        };
    },
	
	created: async function() {
		this.filetree = await api.getFiletree();
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
				<ul class="menu-list">
					<season-folder v-for="folder in filetree"
						:number="folder.number"
						@file-clicked="$emit('file-clicked', ...arguments)"
						:files="folder.files"
						:filter="fileFilter"
					></season-folder>
				</ul>
			</aside>
		</div>
	</div>`
});