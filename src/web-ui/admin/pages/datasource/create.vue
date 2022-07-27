<template>
  <v-card>
    <v-card-title>
      {{ $t("Create New Datasource") }}
      <v-spacer></v-spacer>
      <v-btn text color="primary" nuxt :to="uri">
        <v-icon>mdi-arrow-left-bold</v-icon>
        {{ $t("Back to Datasource List") }}
      </v-btn>
    </v-card-title>
    <v-card-text>
      <v-form ref="form" v-model="valid" class="pb-2">
        <v-text-field v-model="item.name" :rules="nameRules()" :label="$t('Name')" required clearable>
        </v-text-field>

        <v-select @change="onTypeSelected" v-model="component" :rules="typeRules()" :items="typeOptions"
          item-text="displayName" item-value="component" :label="$t('Datasource Type')">
        </v-select>
        <component :is="component" :options="item.options" :entries="entries" :config="configs[item.type]" />

        <v-textarea v-model="item.comment" :label="$t('Comment')"></v-textarea>
      </v-form>
    </v-card-text>

    <v-card-actions>
      <v-btn color="info" :disabled="!valid" @click="save">
        <v-icon>mdi-content-save-outline</v-icon>{{ $t("Save") }}
      </v-btn>
      <v-btn color="info" :disabled="!valid" @click="save(true)">
        <v-icon>mdi-content-save-outline</v-icon>{{ $t("Save & Continue") }}
      </v-btn>
      <v-spacer></v-spacer>
      <v-btn :disabled="!valid" color="warning" @click="cancel">
        <v-icon>mdi-cancel</v-icon>{{ $t("Cancel") }}
      </v-btn>
    </v-card-actions>
  </v-card>
</template>
<script>
import _ from "lodash";

export default {
  async fetch() {

    const res = await this.$axios({ url: this.uri });
    this.entries = res.data.map(x => {
      return {
        ...x
      };
    });
  },
  data() {
    return {
      uri: "/datasource/",
      item: {},
      component: "",
      entries: [],
      typeOptions: [],
      loading: true,
      valid: false,
      configs: []
    };
  },
  methods: {
    onTypeSelected(v) {
      console.log("this is the selected type:", v)
    },
    nameRules() {
      return [
        (v) => !!v || this.$t("Name is required"),
        (v) =>
          (!!v && v.length > 3) ||
          this.$t("Name must be least 3 characters long"),
        (v) => {
          const res = this.entries.find((a) => a.name == v);
          return !res || this.$t("Name must be unique");
        },
      ];
    },
    typeRules() {
      return [(v) => !!v || this.$t("Datasource type is required")];
    },
    async save(continueEdit) {
      this.saving = true;

      const { data } = await this.$axios(
        {
          url: this.uri,
          data: this.item,
          method: 'POST'
        });
      this.saving = false;

      let nextRoute = this.uri;
      if (typeof continueEdit == "boolean" && continueEdit) {
        nextRoute += `/${data.id}`;
      }
      this.$router.push(nextRoute);
    },
    cancel() {
      this.$router.push(this.uri);
    },
  },
};
</script>