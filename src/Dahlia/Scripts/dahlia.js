var global = this;
(function () {
    //
    var Retreat = Backbone.Model.extend({
        initialize: function () {
        }
    });

    var RetreatCollection = Backbone.Collection.extend({
        model: Retreat,
        localStore: new Store("retreats")
    });

    var retreatsStore = new RetreatCollection();

    var RetreatView = Backbone.View.extend({
        retreatsTempl: null,
        initialize: function () {
            this.retreatsTempl = _.template($('#retreat_templ').html());
        },

        render: function() {
            var m = this.model.toJSON();
            $(this.el).html(this.retreatsTempl(m));
            $(this.el).css('display', 'inline');
            return this;
        }
    });

    global.RetreatsUI = Backbone.View.extend({
        initialize: function () {
            retreatsStore.bind('add', this.addRetreat);
            retreatsStore.bind('all', this.render);

            var retreats = this.options.rawRetreats;

            _.each(retreats, function (r) {
                r.id = r.StartDateStr;
                retreatsStore.add(r);
            });
        },

        render: function () {
            return this;
        },

        addRetreat: function (retreat) {
            var view = new RetreatView({model: retreat});
            $('#retreats_list').append(view.render().el);
        }

    });

})()
