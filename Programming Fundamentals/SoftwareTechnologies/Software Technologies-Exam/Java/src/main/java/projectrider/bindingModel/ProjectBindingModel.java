package projectrider.bindingModel;

import javax.validation.constraints.NotNull;

public class ProjectBindingModel {

    @NotNull
    private String title;
    @NotNull
    private String description;
    @NotNull
    private Integer Budget;

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public Integer getBudget() {
        return Budget;
    }

    public void setBudget(Integer budget) {
        Budget = budget;
    }
}
