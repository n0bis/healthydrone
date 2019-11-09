package API.Resources;

import API.Domain.Models.DroneMission;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.List;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class MissionResource {

    private List<DroneMission> missions;
    private Boolean returnToHomeAfterMission;
    private String operationId;

}
