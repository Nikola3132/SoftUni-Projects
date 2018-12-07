const Project = require('../models').Project;

module.exports = {
    index: (req, res) => {
        Project
            .findAll()
            .then(projects => res.render('project/index',
                {projects: projects}))
    },
    createGet: (req, res) => {
        res.render('project/create');
    },
    createPost: (req, res) => {
        let projectArgs = req.body.project;

        Project
            .create(projectArgs)
            .then(() => res.redirect('/'));
    },
    editGet: (req, res) => {
        let id = req.params.id;

        Project
            .findById(id)
            .then(projet => res.render('project/edit', {project: projet}));
    },

    editPost: (req, res) => {
        let id = req.params.id;
        let projectArgs = req.body.project;

        Project
            .findById(id)
            .then(project => project
                .updateAttributes(projectArgs)
                .then(() => res.redirect('/')));
    },

    deleteGet: (req, res) => {
        let id = req.params.id;

        Project
            .findById(id)
            .then(project => res.render('project/delete', {project: project}));


    },
    deletePost: (req, res) => {
        let id = req.params.id;

        Project
            .findById(id)
            .then(project => project
                .destroy()
                .then(() => res.redirect('/')));
    }

};