using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inq.BPMN.Types
{
    public enum eBPMNShapeType
    {
        BPMNShape,
        BPMNEventStart,
        BPMNEventIntermediate,
        BPMNEventEnd,
        BPMNGateway,
        BPMNPool,
        BPMNLane,
        BPMNTopLevel
    }

    public enum eDrawingCategory
    {
        BasicShape,
        StartEvent,
        EventIntermediate,
        EndEvent,
        Gateway
    }

    public enum eBasicShapes
    {
        Pool,
        Lane,
        Activity
    }

    public enum eEventStart
    {
        EventStartNone,
        EventStartMessage,
        EventStartTimer,
        EventStartRule,
        EventStartLink,
        EventStartMultiple
    }

    public enum eEventIntermediate
    {

        EventIntermediateNone,
        EventIntermediateMessage,
        EventIntermediateTimer,
        EventIntermediateError,
        EventIntermediateCancel,
        EventIntermediateCompensation,
        EventIntermediateLink,
        EventIntermediateMultiple,
        EventIntermediateRule
    }

    public enum eEventEnd
    {
        EventEndNone,
        EventEndMessage,
        EventEndError,
        EventEndCancel,
        EventEndCompensation,
        EventEndLink,
        EventEndMultiple,
        EventEndTerminate
    }

    public enum eGateway
    {
        GatewayNone,
        GatewayXorDatabased,
        GatewayXorEventbased,
        GatewayOr,
        GatewayComplex,
        GatewayAnd
    }

    public enum eConnectingObject
    {
        ConnectingMessage,
        ConnectingSequence,
        ConnectingAssociation
    }

    public enum eMouseState
    {
        Drawing,
        Moving,
        Connecting,
        Resizing
    }
}
