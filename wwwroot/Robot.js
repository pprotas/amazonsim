//Javascript enum
const LoadStates =
    Object.freeze(
        {
            "NOT_LOADING": 1,
            "LOADING": 2,
            "LOADED": 3
        });

class Robot extends THREE.Group {

    /**
     * Constructor voor het aanmaken van een Robot
     */
    constructor() {
        super();

        this._loadState = LoadStates.NOT_LOADING;

        this.init();
    }

    /**
     * Geeft de loadstate van de Robot terug
     */
    get loadState() {
        return this._loadState;
    }

    /**
     * Voegt textures en pointlights toe aan de Robot
     */
    init() {
         if(this._loadState != LoadStates.NOT_LOADING){
             return;
         }

         this._loadState = LoadStates.LOADING;

         var selfRef = this;
         loadOBJModel("models/", "DomesticRobot_1293.obj", "textures/", "DomesticRobot_1293.mtl", (mesh) => {
            mesh.scale.set(0.05, 0.05, 0.05);
            mesh.castShadow = true;
            mesh.material = new THREE.MeshPhongMaterial;
            selfRef.add(mesh);

            addPointLight(selfRef, 0xff4751, 0, 0.75, 0, 2, 7);

            selfRef._loadState = LoadStates.LOADED;
        });
    }
}

/**
 * Maakt een pointlight aan
 * @param {*} object Het 3D model die de pointlights krijgt
 * @param {*} color De kleur van de pointlight 
 * @param {*} x X-coordinaat
 * @param {*} y Y
 * @param {*} z Z
 * @param {*} intensity Intensiteit van het licht
 * @param {*} distance Schijnafstand van het licht
 */
function addPointLight(object, color, x, y, z, intensity, distance) {
    var light = new THREE.PointLight(color, intensity, distance);
    light.position.set(x, y, z);
    object.add(light);
}