window.$locator = (function Locator() {
	const services = {};
	const singletons = {};
	return {
		service,
		locate,
		run
	};
	
	/*
		Register a new service
		name: Unique name that the service will be injected assign
		definition: An object, or function that returns an object, in the following form:

		{
			//	A list of service names that should be injected when the service is instantiated
			inject: [],

			//	The service object itself
			//	Services registered this way will be instantiated as singletons
			instance: { },

			// OR

			//	A function that will be invoked to return the service object
			//	It will be passed one parameter; an object containing the injected services (keyed by name)
			instance: function(dependencies) { }
		}
	*/
	function service(name, definition) {
		if (name == undefined) throw 'arg-null: name';
		if (definition == undefined) throw 'arg-null: definition';
		if (name in services) throw `service ${name} is already registered`;
		
		services[name] = value(definition);
	}
	
	/* Locate a service by its name */
	function locate(name) {
		let result = singletons[name];
		if (result !== undefined)
			return result;
			
		const def = services[name];
		if (def === undefined) throw `couldn't locate service ${name}`;


		const deps = {};
		_.each(def.inject, x => deps[x] = locate(x));
		if (def.instance.constructor == Function)
			result = def.instance(deps);
		else
			singletons[name] = result = def.instance;
			
		_.assign(result, deps);
		return result;
	}

	/*
		Run a block of code with the ability to inject services
		inject: An array of service names that should be injected when the code is Run
		callback: A function that will be run with the specified services
				  The services will be passed as parameters in the order that they're specified in the 'inject' array
	*/
	function run(inject, callback) {
		if (inject == null) throw 'arg-null: inject';
		if (callback == null) throw 'arg-null: callback';

		callback.apply(null, _.map(inject, locate));
	}

	function value(objOrFunc, args) {
		if (objOrFunc == null) return null;
		else if (objOrFunc.constructor == Function) return objOrFunc.apply();
		else return objOrFunc;
	}
})();