function parseCommand(input = "") {
    return JSON.parse(input);
}

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

var exampleSocket;

window.onload = function () {
    var camera, scene, renderer;
    var cameraControls;
    var worldObjects = {};
    var robots = [];
    var lights = new THREE.Group();
    
    function init() {
        camera = new THREE.PerspectiveCamera(70, window.innerWidth / window.innerHeight, 1, 1500);
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

        var skyboxGeometry = new THREE.SphereGeometry(1000, 32, 32);
        var skyboxMaterial = new THREE.MeshBasicMaterial({ map: new THREE.TextureLoader().load("textures/SkyBox.jpg"), side: THREE.DoubleSide });
        var skybox = new THREE.Mesh(skyboxGeometry, skyboxMaterial);
        scene.add(skybox);

        var light = new THREE.PointLight(0xffffff, 2.5);
        lights.add(light);
        light.position.set(10, 4, 10);
        var pointLightHelper = new THREE.PointLightHelper(light, 1, 0xff0000);
        lights.add(pointLightHelper);
        light2 = new THREE.AmbientLight(0x404040, 2);
        lights.add(light2);
        scene.add(lights);

        var material = new THREE.ShaderMaterial( {
            uniforms: {
                time: { value: 1.0 },
            },
            vertexShader: document.getElementById( 'rackVertexShader' ).textContent
        })
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
                    var robot = new Robot();
                    /*loadOBJModel("models/", "DomesticRobot_1293.obj", "textures/", "DomesticRobot_1293.mtl", (mesh) => {
                        mesh.scale.set(0.05, 0.05, 0.05);
                        robot.add(mesh);
                        robot.material = new THREE.MeshPhongMaterial;
                        models.add(robot);
                    });*/
                    
                    models.add(robot);
                    worldObjects[command.parameters.guid] = robot;
                    //robots.push(worldObjects[command.parameters.guid]);
                }
                else if (command.parameters.type == "rack") {
                    var rack = new THREE.Group();
                    loadOBJModel("models/", "CatModel.obj", "textures/", "CatModel.mtl", (mesh) => {
                        mesh.scale.set(0.005, 0.005, 0.005);
                        mesh.rotation.set(Math.PI * 1.5, 0, 0);
                        rack.add(mesh);
                        models.add(rack);
                    });
                    worldObjects[command.parameters.guid] = rack;
                }
                else if (command.parameters.type == "truck") {
                    var truck = new Van();
                    /*loadOBJModel("models/", "Van.obj", "textures/", "Van.mtl", (mesh) => {
                        mesh.scale.set(0.1, 0.1, 0.1);
                        truck.add(mesh);
                        models.add(truck);
                    });*/
                    models.add(truck);
                    worldObjects[command.parameters.guid] = truck;
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