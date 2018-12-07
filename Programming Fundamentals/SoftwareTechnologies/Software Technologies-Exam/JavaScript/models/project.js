const Sequelize = require('sequelize');

module.exports = function (sequelize) {
    let Project = sequelize.define('Project', {
        Title: {
            type: Sequelize.TEXT,
            allowNull: false
        },
        Description: {
            type: Sequelize.TEXT,
            allowNull: false
        },
        Budget: {
            type: Sequelize.INTEGER,
            allowNull: false
        }
    });

    return Project;
};