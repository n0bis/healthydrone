import React, { Component } from "react";
import { inject, observer } from "mobx-react";
import { Button } from "antd";

@inject("reportStore")
@observer
class ReportIcon extends Component {

    render(){ 
        const {onOpenModal} = this.props.reportStore;
        return(
        <div>
        <Button type="danger" onClick={ onOpenModal } >Report Incident</Button>
        </div>   
    );
    }
}

export default ReportIcon;