$locator.run(['$workspace'], function($workspace) {
	Vue.component('workspace', {
		props: [],
		data: () => $workspace,

		computed: {
			filteredFiles: function() {
				const upFilter = (this.filter || '').toUpperCase();
				return _.filter(this.files, x => x.toUpperCase().indexOf(upFilter) >= 0);
			}
		},
		template: `
		<div class="flex-panel" v-cloak>
			<p class="title is-5">Workspace</p>
			<div class="tabs is-boxed">
				<ul>
					<li @click="focusTab(index)" v-bind:class="{ 'is-active': isActive(file) }" v-for="(file, index) in files">
						<a>
							<span>Season {{file.file.seasonNum}}/{{file.file.filename}}{{ file.isDirty ? '*' : ''}}</span>
							&nbsp;&nbsp;
							<button @click.stop="deleteTab(index)" class="delete is-small"></button>
						</a>
					</li>
				</ul>
			</div>

			<div class="inner" v-if="activeFile != null">
				<div class="hot-table">
					<translation-table
						:strings="activeFile.file.strings"
						@edit="activeFile.isDirty = true"
					></translation-table>
				</div>
				<button @click="saveFile()"
					:disabled="activeFile.isSaving"
					:class="{ 'is-loading': activeFile.isSaving, 'is-danger': activeFile.isError }"
					class="save-button button is-success"
				>
					<span class="icon is-small">
						<i class="fas fa-check"></i>
					</span>
					<span>Save</span>
				</button>
			</div>
			<div v-else>
				Choose a file from the browser on the left
			</div>
		</div>`
	});
});
