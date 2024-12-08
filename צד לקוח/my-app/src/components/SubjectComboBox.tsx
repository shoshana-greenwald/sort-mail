import * as React from 'react';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import Tooltip from '@mui/material/Tooltip';


type subject = {
    subjectId: number;
    subjectName: string;
    parentName: (string | undefined);
    userId: number;
    color: string;
}

type statesList = {
    value: string;
    subjects: subject[];
    setParentFunction: any;
}
export default function ComboBox(props: statesList) {


    const [chooseSubjects, setchooseSubjects] = React.useState<string[]>([]);
    let chooseSubjectsTMP: string[] = [];
    React.useEffect(
        () => {
            let subjects: subject[] = Object.values(props.subjects);
            subjects.forEach(x => {
                chooseSubjectsTMP.push(x.subjectName);
            })
            setchooseSubjects({ ...chooseSubjectsTMP })
        }, [props.subjects]
    )

    return (
        <Tooltip title="שדה לא חובה">
            <Autocomplete
                value={props.value}
                onChange={(event: any, newValue: string | null) => {
                    props.setParentFunction(newValue);
                }}
                disablePortal
                id="combo-box-demo"
                options={Object.values(chooseSubjects)}
                sx={{ width: "10vw", marginRight: "1.6vw" }}
                renderInput={(params) => <TextField {...params} label="נושא אב:" />}
            />
        </Tooltip>
    );
}

