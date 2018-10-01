
function parseCommand(input = "") {
    return JSON.parse(input);
}

var exampleSocket;

window.onload = function () {
    var camera, scene, renderer;
    var cameraControls;
    var worldObjects = {};
    var robots = [];
    var lights = new THREE.Group();

    function loadOBJModel(modelPath, modelName, texturePath, textureName, onload) {
        new THREE.MTLLoader()
            .setPath(texturePath)
            .load(textureName, function (materials) {
                materials.preload();

                new THREE.OBJLoader()
                    .setPath(modelPath)
                    .setMaterials(materials)
                    .load(modelName, function (object) {
                        onload(object);
                    }, function () { }, function (e) { console.log("Error loading model"); console.log(e); });
            });
    }

    function init() {
        camera = new THREE.PerspectiveCamera(70, window.innerWidth / window.innerHeight, 1, 1000);
        cameraControls = new THREE.OrbitControls(camera);
        camera.position.z = 15;
        camera.position.y = 5;
        camera.position.x = 15;
        cameraControls.update();
        scene = new THREE.Scene();

        renderer = new THREE.WebGLRenderer({ antialias: true });
        renderer.setPixelRatio(window.devicePixelRatio);
        renderer.setSize(window.innerWidth, window.innerHeight + 5);
        document.body.appendChild(renderer.domElement);

        window.addEventListener('resize', onWindowResize, false);

        var geometry = new THREE.PlaneGeometry(30, 30, 32);
        var material = new THREE.MeshPhongMaterial({ color: 0xffffff, side: THREE.DoubleSide });
        var plane = new THREE.Mesh(geometry, material);
        plane.rotation.x = Math.PI / 2.0;
        plane.position.x = 0;
        plane.position.z = 0;
        scene.add(plane);


        var light = new THREE.PointLight(0xffffff, 2.5);
        lights.add(light);
        light.position.set(10, 4, 10);
        var pointLightHelper = new THREE.PointLightHelper(light, 1, 0xff0000);
        lights.add(pointLightHelper);
        light2 = new THREE.AmbientLight(0x404040, 2);
        lights.add(light2);
        scene.add(lights);
    }

    function onWindowResize() {
        camera.aspect = window.innerWidth / window.innerHeight;
        camera.updateProjectionMatrix();
        renderer.setSize(window.innerWidth, window.innerHeight);
    }

    function animate() {
        requestAnimationFrame(animate);
        cameraControls.update();
        renderer.render(scene, camera);
        lights.updateMatrix();
        lights.updateMatrixWorld();
    }

    exampleSocket = new WebSocket("ws://" + window.location.hostname + ":5000" + "/connect_client");
    exampleSocket.onmessage = function (event) {
        var command = parseCommand(event.data);
        if (command.command == "update") {
            var models = new THREE.Group();
            scene.add(models);
            if (Object.keys(worldObjects).indexOf(command.parameters.guid) < 0) {
                if (command.parameters.type == "robot") {
                    var robot = new THREE.Group();
                    loadOBJModel("models/", "DomesticRobot_1293.obj", "textures/", "DomesticRobot_1293.mtl", (mesh) => {
                        mesh.scale.set(0.05, 0.05, 0.05);
                        robot.add(mesh);
                        robot.material = new THREE.MeshPhongMaterial;
                        models.add(robot);
                    });
                    worldObjects[command.parameters.guid] = robot;
                    robots.push(worldObjects[command.parameters.guid]);
                }
                if (command.parameters.type == "rack") {
                    var rack = new THREE.Group();
                    loadOBJModel("models/", "CatModel.obj", "textures/", "CatModel.mtl", (mesh) => {
                        mesh.scale.set(0.005, 0.005, 0.005);
                        mesh.rotation.set(Math.PI * 1.5, 0, 0);
                        rack.add(mesh);
                        models.add(rack);
                    });
                    worldObjects[command.parameters.guid] = rack;
                }
            }
            var object = worldObjects[command.parameters.guid];

            object.position.x = command.parameters.x;
            object.position.y = command.parameters.y;
            object.position.z = command.parameters.z;

            object.rotation.x = command.parameters.rotationX;
            object.rotation.y = command.parameters.rotationY;
            object.rotation.z = command.parameters.rotationZ;
        }
    }
    init();
    animate();
}
